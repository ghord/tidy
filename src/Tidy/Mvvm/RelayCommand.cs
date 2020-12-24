using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tidy.Mvvm
{
    /// <summary>
    /// Command that invokes an action
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<bool>? canExecute_;
        private readonly Func<Task> task_;
        private readonly bool blockExecution_;
        private bool executing_;

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a command that runs the action on execute
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="canExecute">Predicate checked before action execution</param>
        public RelayCommand(Action action, Func<bool>? canExecute = null)
        {
            canExecute_ = canExecute;
            task_ = () => { action(); return Task.CompletedTask; };
        }

        /// <summary>
        /// Creates a command that runs the async action on execute
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="canExecute">Predicate checked before action execution</param>
        /// <param name="blockExecution">If true, only one instance of command can be executing at any given time</param>
        public RelayCommand(Func<Task> action, Func<bool>? canExecute = null, bool blockExecution = true)
        {
            task_ = action;
            canExecute_ = canExecute;
            blockExecution_ = blockExecution;
        }

        bool ICommand.CanExecute(object? parameter)
        {
            return CanExecute();
        }

        void ICommand.Execute(object? parameter)
        {
            Execute();
        }

        /// <inheritdoc cref="ICommand.CanExecute(object?)"/>
        public bool CanExecute()
        {
            if (blockExecution_ && executing_)
                return false;

            if (canExecute_?.Invoke() == false)
                return false;

            return true;
        }

        /// <inheritdoc cref="ICommand.Execute(object?)"/>
        public void Execute()
        {
            if (blockExecution_)
            {
                executing_ = true;
                CommandManager.InvalidateRequerySuggested();
            }

            try
            {
                var frame = new DispatcherFrame();
                Exception? exception = null;

                task_().ContinueWith(task =>
                {
                    frame.Continue = false;

                    if (task.IsFaulted)
                        exception = task.Exception!;
                });

                Dispatcher.PushFrame(frame);

                if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                    ExceptionDispatchInfo.Throw(aggregateException.InnerExceptions.Single());
                else if (exception is not null)
                    ExceptionDispatchInfo.Throw(exception);
            }
            finally
            {
                if (blockExecution_)
                {
                    executing_ = false;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
    }

    /// <summary>
    /// Command that invokes an action with parameter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool>? canExecute_;
        private readonly Func<T, Task> task_;
        private readonly bool blockExecution_;
        private bool executing_;

        /// <inheritdoc />
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a command that runs the action on execute
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="canExecute">Predicate checked before action execution</param>
        public RelayCommand(Action<T> action, Func<T, bool>? canExecute = null)
        {
            canExecute_ = canExecute;
            task_ = param => { action(param); return Task.CompletedTask; };
        }

        /// <summary>
        /// Creates a command that runs the async action on execute
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="canExecute">Predicate checked before action execution</param>
        /// <param name="blockExecution">If true, only one instance of command can be executing at any given time</param>
        public RelayCommand(Func<T, Task> action, Func<T, bool>? canExecute = null, bool blockExecution = true)
        {
            task_ = action;
            canExecute_ = canExecute;
            blockExecution_ = blockExecution;
        }

        /// <inheritdoc />
        public bool CanExecute(object? parameter)
        {
            if (blockExecution_ && executing_)
                return false;

            if (parameter is not T typedParameter || canExecute_?.Invoke(typedParameter) == false)
                return false;

            return true;
        }

        /// <inheritdoc />
        public void Execute(object? parameter)
        {
            if (parameter is not T typedParameter)
                return;

            if (blockExecution_)
            {
                executing_ = true;
                CommandManager.InvalidateRequerySuggested();
            }

            try
            {
                var frame = new DispatcherFrame();
                Exception? exception = null;

                task_(typedParameter).ContinueWith(task =>
                {
                    frame.Continue = false;

                    if (task.IsFaulted)
                        exception = task.Exception!;
                });

                Dispatcher.PushFrame(frame);

                if (exception is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1)
                    ExceptionDispatchInfo.Throw(aggregateException.InnerExceptions.Single());
                else if (exception is not null)
                    ExceptionDispatchInfo.Throw(exception);
            }
            finally
            {
                if (blockExecution_)
                {
                    executing_ = false;
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
    }
}
