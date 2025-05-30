// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Media.Animation;
using System.Windows.Markup;

namespace System.Windows.Media
{
    #region PathFigure
    /// <summary>
    /// PathFigure
    /// </summary>
    [ContentProperty("Segments")]
    [Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
    public sealed partial class PathFigure : Animatable, IFormattable
    {
        #region Constructors
        /// <summary>
        ///
        /// </summary>
        public PathFigure()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">The path's startpoint</param>
        /// <param name="segments">A collection of segments</param>
        /// <param name="closed">Indicates whether the figure is closed</param>
        public PathFigure(Point start, IEnumerable<PathSegment> segments, bool closed)
        {
            StartPoint = start;
            PathSegmentCollection mySegments = Segments;

            ArgumentNullException.ThrowIfNull(segments);

            foreach (PathSegment item in segments)
            {
                mySegments.Add(item);
            }

            IsClosed = closed;
        }

        #endregion Constructors

        #region GetFlattenedPathFigure
        /// <summary>
        /// Approximate this figure with a polygonal PathFigure
        /// </summary>
        /// <param name="tolerance">The approximation error tolerance</param>
        /// <param name="type">The way the error tolerance will be interpreted - relative or absolute</param>
        /// <returns>Returns the polygonal approximation as a PathFigure.</returns>
        public PathFigure GetFlattenedPathFigure(double tolerance, ToleranceType type)
        {
            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(this);

            PathGeometry flattenedGeometry = geometry.GetFlattenedPathGeometry(tolerance, type);

            int count = flattenedGeometry.Figures.Count;

            if (count == 0)
            {
                return new PathFigure();
            }
            else if (count == 1)
            {
                return flattenedGeometry.Figures[0];
            }
            else
            {
                throw new InvalidOperationException(SR.PathGeometry_InternalReadBackError);
            }
        }


        /// <summary>
        /// Approximate this figure with a polygonal PathFigure
        /// </summary>
        /// <returns>Returns the polygonal approximation as a PathFigure.</returns>
        public PathFigure GetFlattenedPathFigure()
        {
            return GetFlattenedPathFigure(Geometry.StandardFlatteningTolerance, ToleranceType.Absolute);
        }

        #endregion

        /// <summary>
        /// Returns true if this geometry may have curved segments
        /// </summary>
        public bool MayHaveCurves()
        {
            PathSegmentCollection segments = Segments;

            if (segments == null)
            {
                return false;
            }

            int count = segments.Count;

            for (int i = 0; i < count; i++)
            {
                if (segments.Internal_GetItem(i).IsCurved())
                {
                    return true;
                }
            }

            return false;
        }

        #region GetTransformedCopy
        internal PathFigure GetTransformedCopy(Matrix matrix)
        {
            PathSegmentCollection segments = Segments;

            PathFigure result = new PathFigure();
            Point current = StartPoint;
            result.StartPoint = current * matrix;

            if (segments != null)
            {
                int count = segments.Count;
                for (int i=0; i<count; i++)
                {
                    segments.Internal_GetItem(i).AddToFigure(matrix, result, ref current);
                }
            }

            result.IsClosed = IsClosed;
            result.IsFilled = IsFilled;

            return result;
        }
        #endregion

        /// <summary>
        /// Creates a string representation of this object based on the current culture.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public override string ToString()
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(format: null, provider: null);
        }

        /// <summary>
        /// Creates a string representation of this object based on the IFormatProvider
        /// passed in.  If the provider is null, the CurrentCulture is used.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(format: null, provider);
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string 
        /// and IFormatProvider passed in.  
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        string IFormattable.ToString(string format, IFormatProvider provider)
        {
            ReadPreamble();
            // Delegate to the internal method which implements all ToString calls.
            return ConvertToString(format, provider);
        }

        /// <summary>
        /// Can serialze "this" to a string.
        /// This returns true iff IsFilled == c_isFilled and the segment
        /// collection can be stroked.
        /// </summary>
        internal bool CanSerializeToString()
        {
            PathSegmentCollection segments = Segments;
            return (IsFilled == c_IsFilled) && ((segments == null) || segments.CanSerializeToString());
        }

        /// <summary>
        /// Creates a string representation of this object based on the format string 
        /// and IFormatProvider passed in.  
        /// If the provider is null, the CurrentCulture is used.
        /// See the documentation for IFormattable for more information.
        /// </summary>
        /// <returns>
        /// A string representation of this object.
        /// </returns>
        internal string ConvertToString(string format, IFormatProvider provider)
        {
            PathSegmentCollection segments = Segments;
            return "M" + 
                ((IFormattable)StartPoint).ToString(format, provider) + 
                (segments != null ? segments.ConvertToString(format, provider) : "") +
                (IsClosed ? "z" : "");
        }
 
        /// <summary>
        /// SerializeData - Serialize the contents of this Figure to the provided context.
        /// </summary>
        internal void SerializeData(StreamGeometryContext ctx)
        {
            ctx.BeginFigure(StartPoint, IsFilled, IsClosed);

            PathSegmentCollection segments = Segments;

            int pathSegmentCount = segments == null ? 0 : segments.Count;

            for (int i = 0; i < pathSegmentCount; i++)
            {
                segments.Internal_GetItem(i).SerializeData(ctx);
            }
        }
    }

    #endregion
}

