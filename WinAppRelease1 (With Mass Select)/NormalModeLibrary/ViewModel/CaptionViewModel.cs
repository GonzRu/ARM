using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NormalModeLibrary.ViewModel
{
    public class CaptionViewModel : ViewModelBase
    {
        private string _captionText;
        private UInt16 _fontSize;

        public string CaptionText
        {
            get { return _captionText; }
            set { _captionText = value; OnPropertyChanged("caption"); }
        }

        public override UInt16 FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        public CaptionViewModel(string caption)
        {
            _captionText = caption;
        }

        internal override ViewModelBase Copy()
        {
            return new CaptionViewModel(_captionText);
        }
    }
}
