using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Reline.Tests;

public static class AssertAsync {

	/// <summary>
	/// Runs an <see cref="Action"/> and throws a
	/// <see cref="TimeoutException"/> if the action
	/// did not complete in a specified time interval.
	/// </summary>
	/// <param name="timeout">The time interval after which
	/// the method will throw a <see cref="TimeoutException"/>.</param>
	/// <param name="action">The action to run.</param>
	/// <exception cref="TimeoutException">
	/// <paramref name="action"/> did not complete in
	/// the <paramref name="timeout"/> interval.
	/// </exception>
	public static void CompletesIn(TimeSpan timeout, Action action) {
		var task = Task.Run(action);
		bool completedInTime = Task.WaitAll(new[] { task }, timeout);

		if (task.Exception is not null) {
			throw task.Exception.InnerExceptions.Count == 1 ?
				task.Exception.InnerExceptions[0] : task.Exception;
		}

		if (!completedInTime)
			throw new TimeoutException($"Task did not complete in {timeout} seconds.");
	}
	/// <summary>
	/// Runs an <see cref="Action"/> and throws a
	/// <see cref="TimeoutException"/> if the action
	/// did not complete in a specified time interval.
	/// </summary>
	/// <param name="timeout">The time interval specified in milliseconds after which
	/// the method will throw a <see cref="TimeoutException"/>.</param>
	/// <param name="action">The action to run.</param>
	/// <exception cref="TimeoutException">
	/// <paramref name="action"/> did not complete in
	/// the <paramref name="timeout"/> interval.
	/// </exception>
	public static void CompletesIn(double timeout, Action action) =>
		CompletesIn(TimeSpan.FromMilliseconds(timeout), action);

	/// <summary>
	/// Runs a <see cref="Func{TResult}"/> and throws a
	/// <see cref="TimeoutException"/> if the function
	/// did not complete in a specified time interval.
	/// </summary>
	/// <typeparam name="TResult">The return type of the function to run.</typeparam>
	/// <param name="timeout">The time interval after which
	/// the method will throw a <see cref="TimeoutException"/>.</param>
	/// <param name="func">The function to run.</param>
	/// <returns>The return value of <paramref name="func"/>.</returns>
	/// <exception cref="TimeoutException">
	/// <paramref name="func"/> did not complete in
	/// the <paramref name="timeout"/> interval.
	/// </exception>
	public static TResult CompletesIn<TResult>(TimeSpan timeout, Func<TResult> func) {
		var task = Task.Run(func);
		bool completedInTime = Task.WaitAll(new[] { task }, timeout);

		if (task.Exception is not null) {
			throw task.Exception.InnerExceptions.Count == 1 ?
				task.Exception.InnerExceptions[0] : task.Exception;
		}

		if (!completedInTime && !Debugger.IsAttached)
			throw new TimeoutException($"Task did not complete in {timeout} seconds.");

		return task.Result;
	}
	/// <summary>
	/// Runs a <see cref="Func{TResult}"/> and throws a
	/// <see cref="TimeoutException"/> if the function
	/// did not complete in a specified time interval.
	/// </summary>
	/// <typeparam name="TResult">The return type of the function to run.</typeparam>
	/// <param name="timeout">The time interval specified in milliseconds after which
	/// the method will throw a <see cref="TimeoutException"/>.</param>
	/// <param name="func">The function to run.</param>
	/// <returns>The return value of <paramref name="func"/>.</returns>
	/// <exception cref="TimeoutException">
	/// <paramref name="func"/> did not complete in
	/// the <paramref name="timeout"/> interval.
	/// </exception>
	public static TResult CompletesIn<TResult>(double timeout, Func<TResult> func) =>
		CompletesIn<TResult>(TimeSpan.FromMilliseconds(timeout), func);

}
