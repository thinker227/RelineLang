using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reline.Compilation.Symbols;

namespace Reline.Compilation.Binding;

/// <summary>
/// Binds variables.
/// </summary>
internal sealed class VariableBinder {

	public VariableBinder() {

	}



	public void RegisterVariable(string identifier, ITypeSymbol type) {
		throw new NotImplementedException();
	}

	public VariableSymbol GetVariable(string identifier) {
		throw new NotImplementedException();
	}

}
