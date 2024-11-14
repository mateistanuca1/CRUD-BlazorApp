using Microsoft.AspNetCore.Components;

namespace BlazorApp2.Components.Pages
{
    public class CounterBase : ComponentBase
    {
        public int currentCount = 0;

        public void IncrementCount()
        {
            currentCount++;
        }
    }
}
