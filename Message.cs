using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTestApp
{
    class Message
    {

        private string text;
        private float fromTop;
        private float fromLeft;
        
        public Message(string Text)
        {
            
            this.text = text;
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public string getText()
        {
            return this.text;
        }

        public void setFromTop(float fromTop)
        {
            this.fromTop = fromTop;
        }

        public float getFromTop()
        {
            return this.fromTop;
        }

        public void setFromLeft(float fromLeft)
        {
            this.fromLeft = fromLeft;
        }

        public float getFromLeft()
        {
            return this.fromLeft;
        }
    }
}
