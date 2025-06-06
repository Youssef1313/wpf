// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


//---------------------------------------------------------------------------
//

//
// Description: Emits a partial class which contains the implementation
//              of the IAnimatable interface.
//              
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

using MS.Internal.MilCodeGen;
using MS.Internal.MilCodeGen.Runtime;
using MS.Internal.MilCodeGen.ResourceModel;
using MS.Internal.MilCodeGen.Helpers;

namespace MS.Internal.MilCodeGen.ResourceModel
{
    public class AnimatableTemplate : Template
    {
        private struct AnimatableTemplateInstance
        {
            public AnimatableTemplateInstance(
                string mangedDestinationDir,
                string className,
                string @namespace)
            {
                ManagedDestinationDir = mangedDestinationDir;
                ClassName = className;
                Namespace = @namespace;
            }

            public readonly string ManagedDestinationDir;
            public readonly string ClassName;
            public readonly string Namespace;
        }

        public override void AddTemplateInstance(ResourceModel resourceModel, XmlNode node)
        {
            Instances.Add(new AnimatableTemplateInstance(
                ResourceModel.ToString(node, "ManagedDestinationDir"),
                ResourceModel.ToString(node, "ClassName"),
                ResourceModel.ToString(node, "Namespace")
                ));
        }

        public override void Go(ResourceModel resourceModel)
        {
            foreach(AnimatableTemplateInstance instance in Instances)
            {
                string fullPath = Path.Combine(resourceModel.OutputDirectory, instance.ManagedDestinationDir);
                string filename = instance.ClassName + ".cs";

                using (FileCodeSink csFile = new FileCodeSink(fullPath, filename, true /* Create dir if necessary */))
                {
                    csFile.WriteBlock(
                        [[inline]]
                            [[Helpers.ManagedStyle.WriteFileHeader(filename, @"wpf\src\Graphics\codegen\mcg\generators\AnimatableTemplate.cs")]]

                            using System.Windows.Media.Animation;

                            namespace [[instance.Namespace]]
                            {
                                /// <summary>
                                /// This class derives from Freezable and adds the ability to animate properties.
                                /// </summary>
                                public abstract partial class [[instance.ClassName]] : IAnimatable
                                {   
                                    [[IAnimatableHelper.WriteImplementation()]]
                                }
                            }
                        [[/inline]]
                        );
                }
            }
        }
    
        private readonly List<AnimatableTemplateInstance> Instances = new List<AnimatableTemplateInstance>();
    }
}


