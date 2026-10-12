// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Mocha.Models;
using Synesthesia.Utils.Events;

namespace Mocha.Components;

public class MochaContext
{
    public User? User { get; private set; }
    public Day? Day { get; private set; }

    public MochaTask? RequestedTaskEdit { get; private set; }
    public MochaProject? RequestedProjectEdit { get; private set; }

    public Day.FocusedTask? FocusedTask { get; private set; }

    private readonly EventDispatcher<MochaContext> backingEventDispatcher = new EventDispatcher<MochaContext>();

    public IDisposable Subscribe(Action<MochaContext> update)
    {
        var subscriber = backingEventDispatcher.Subscribe(update);
        update.Invoke(this);

        return subscriber;
    }

    public void SetUser(User? user)
    {
        User = user;
        NotifyChanged();
    }

    public void SetDay(Day? day)
    {
        Day = day;
        FocusedTask = day?.CurrentlyFocusedTask;
        NotifyChanged();
    }

    public void SetFocused(Day.FocusedTask? focusedTask)
    {
        var newDay = Day! with { CurrentlyFocusedTask = focusedTask };
        Day.DATABASE_COLLECTION.Insert(newDay.Date, newDay);
        FocusedTask = focusedTask;
        Day = newDay;
        if (focusedTask != null)
        {
            var task = MochaTask.DATABASE_COLLECTION.Find(focusedTask.TaskId);
            var newTask = task with { ElapsedTime = focusedTask.TotalElapsed };
            MochaTask.DATABASE_COLLECTION.Insert(newTask.Id, newTask);
        }
        NotifyChanged();
    }


    public void RequestTaskEdit(MochaTask? task)
    {
        RequestedTaskEdit = task;
        NotifyChanged();
    }

    public void RequestProjectEdit(MochaProject? task)
    {
        RequestedProjectEdit = task;
        NotifyChanged();
    }


    public void NotifyChanged() => backingEventDispatcher.Dispatch(this);

    public bool Loaded => User != null && Day != null;
}
