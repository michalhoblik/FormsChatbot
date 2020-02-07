using System.Collections.Generic;
using FormsChatbot.Events;
using FormsChatbot.Models;
using Prism.Behaviors;
using Prism.Events;
using Xamarin.Forms;

namespace FormsChatbot.Behaviors
{
    public class ScrollToChatBotMessageBehavior : BehaviorBase<ListView>
    {
        private IEventAggregator _eventAggregator;
        public IEventAggregator EventAggregator
        {
            get => _eventAggregator;
            set
            {
                if (!EqualityComparer<IEventAggregator>.Default.Equals(_eventAggregator, value))
                {
                    _eventAggregator = value;
                    _eventAggregator.GetEvent<ScrollToChatBotMessageEvent>().Subscribe(OnScrollToEventPublished);
                }
            }
        }

        private void OnScrollToEventPublished(ChatBotMessage model)
        {
            AssociatedObject.ScrollTo(model, ScrollToPosition.Start, true);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            EventAggregator.GetEvent<ScrollToChatBotMessageEvent>().Unsubscribe(OnScrollToEventPublished);
        }
    }
}
