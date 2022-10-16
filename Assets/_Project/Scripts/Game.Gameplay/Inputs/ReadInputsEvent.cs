using Leaosoft.Events;

namespace Game.Gameplay.Inputs
{
    public sealed class ReadInputsEvent : ServiceEvent
    {
        public ReadInputsEvent(InputsData inputsData)
        {
            InputsData = inputsData;
        }
        
        public InputsData InputsData { get; }
    }
}