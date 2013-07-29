using Glimpse.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Glimpse
{
    public class TimingMessage : ITimelineMessage
    {
        public Guid Id
        {
            get { throw new NotImplementedException(); }
        }

        public TimelineCategoryItem EventCategory
        {
            get;
            set;
        }

        public string EventName
        {
            get;
            set;
        }

        public string EventSubText
        {
            get;
            set;
        }

        public TimeSpan Duration
        {
            get;
            set;
        }

        public TimeSpan Offset
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }
    }

}
