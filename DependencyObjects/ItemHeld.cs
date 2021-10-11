using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ItemHeldDelay.DependencyObjects
{

	public class ItemHeld : DependencyObject
	{

		public static readonly DependencyProperty HeldCommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemHeld), new PropertyMetadata(null, HeldCommandChanged));

		private static readonly TimeSpan DelayTime = TimeSpan.FromSeconds(1.1);

		private static CancellationTokenSource CancelTimer;


		public ItemHeld()
		{

		}

		public static ICommand GetHeldCommand(DependencyObject bindable)
		{
			return (ICommand)bindable.GetValue(HeldCommandProperty);
		}

		public static void SetHeldCommand(DependencyObject bindable, ICommand value)
		{
			bindable.SetValue(HeldCommandProperty, value);
		}


		private static void HeldCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = (d as FrameworkElement);
			if (element is null) return;

			var command = GetHeldCommand(d);

			element.PreviewMouseUp += (s, e) => CancelTimer.Cancel();
			
			element.PreviewMouseDown += (s, e) => Task.Run(async () =>
			{
				await PreviewMouseDown(s, e, command);
/*
				if (await PreviewMouseDown(s, e, command))
				{
					element.Dispatcher.Invoke(() => command.Execute(null));
				}
*/
			});

		}

		private static async Task<Boolean> PreviewMouseDown(object sender, MouseButtonEventArgs e, ICommand command)
		{
			var element = (sender as FrameworkElement);
			if (element is null) return false;

			CancelTimer = new CancellationTokenSource();

			try
			{
				await Task.Delay(DelayTime, CancelTimer.Token);

				element.Dispatcher.Invoke(() => command.Execute(null));
			}
			catch (TaskCanceledException)
			{
				// user released mouse

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

			return false;
		}





		/*
		public static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = bindable as View;

			if (control != null)
			{
				control.GestureRecognizers.Clear();
				control.GestureRecognizers.Add(
					 new TapGestureRecognizer()
					 {
						 Command = new Command((o) =>
						 {

							 var command = GetItemTapped(control);

							 if (command != null && command.CanExecute(null))
								 command.Execute(null);
						 })
					 }
				);
			}
		}
		*/
	}
}