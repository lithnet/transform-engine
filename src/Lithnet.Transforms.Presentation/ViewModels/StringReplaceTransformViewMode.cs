using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using Lithnet.Common.Presentation;
using System.Collections.ObjectModel;
using Lithnet.MetadirectoryServices;
using Lithnet.Common.ObjectModel;

namespace Lithnet.Transforms.Presentation
{
    public class StringReplaceTransformViewModel : TransformViewModel
    {
        private StringReplaceTransform model;

        public StringReplaceTransformViewModel(StringReplaceTransform model)
            : base(model)
        {
            this.model = model;
        }

        public ObservableCollection<LookupItem> LookupItems
        {
            get
            {
                return this.model.LookupItems;
            }
            set
            {
                model.LookupItems = value;
            }
        }

        public bool IgnoreCase
        {
            get
            {
                return this.model.IgnoreCase;
            }
            set
            {
                this.model.IgnoreCase = value;
            }
        }

        public override string TransformDescription
        {
            get
            {
                return strings.StringReplaceTransformDescription;
            }
        }
    }
}
