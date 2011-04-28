using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace RhinoMocksExtensions
{
    public static class Extensions
    {
        public static IMethodOptions<TR>  SetPropertyAsBehavior<T, TR>(this T mock, Function<T, TR> action) where T : class
        {
            var options = mock.Expect(action);
            
            LastCall.IgnoreArguments();
            LastCall.PropertyBehavior();

            return options;
        }
    }
}
