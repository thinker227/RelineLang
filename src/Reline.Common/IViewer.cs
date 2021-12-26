using System.Collections.Generic;

namespace Reline.Common;

public interface IViewer<out T> : IEnumerable<T> {

	T Current { get; }
	T Next { get; }

	void MoveNext();

}
