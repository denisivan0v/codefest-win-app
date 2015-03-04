﻿using System;
using System.Collections.Generic;
using System.Linq;

using CodeFestApp.DataModel;

using ReactiveUI;

namespace CodeFestApp.ViewModels
{
    public class DayViewModel : ReactiveObject, IRoutableViewModel
    {
        private readonly Day _day;

        public DayViewModel(IScreen hostScreen, Day day)
        {
            _day = day;
            HostScreen = hostScreen;

            NavigateToLectureCommand = ReactiveCommand.Create();

            this.WhenAnyObservable(x => x.NavigateToLectureCommand)
                .Subscribe(x => HostScreen.Router.Navigate.Execute(x));
        }

        public string Title
        {
            get { return _day.Title; }
        }

        public DateTime Date
        {
            get { return _day.Date; }
        }

        public IEnumerable<IGrouping<string, LectureViewModel>> Lectures
        {
            get
            {
                return _day.Lectures
                           .Select(x => new LectureViewModel(HostScreen, x))
                           .GroupBy(x => x.Start.ToString("t"));
            }
        }

        public ReactiveCommand<object> NavigateToLectureCommand { get; private set; }  

        public string UrlPathSegment
        {
            get { return "day"; }
        }

        public IScreen HostScreen { get; private set; }
    }
} 