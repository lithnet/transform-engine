using System;
using System.Collections.Generic;
using System.Linq;
using Lithnet.Transforms;

namespace Lithnet.Transforms.Presentation
{
    public class BinaryGuidTransformViewModel : TransformViewModel
    {
        private BinaryGuidTransform model;

        public BinaryGuidTransformViewModel(BinaryGuidTransform model)
            : base(model)
        {
            this.model = model;
        }

        public string Format
        {
            get
            {
                return model.Format;
            }
            set
            {
                model.Format = value;
            }
        }

        public override string TransformDescription
        {
            get
            {
                return strings.BinaryGuidTransformDescription;
            }
        }
    }
}
