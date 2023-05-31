using System;
using System.Collections.Generic;
using PROJECT_ES.Data;

public class VoteObservable : IObservable<Vote>
{
    private List<IObserver<Vote>> observers = new List<IObserver<Vote>>();

    public IDisposable Subscribe(IObserver<Vote> observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
        return new Unsubscriber(observers, observer);
    }

    public void NotifyObservers(Vote vote)
    {
        foreach (var observer in observers)
        {
            observer.OnNext(vote);
        }
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<Vote>> _observers;
        private IObserver<Vote> _observer;

        public Unsubscriber(List<IObserver<Vote>> observers, IObserver<Vote> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}