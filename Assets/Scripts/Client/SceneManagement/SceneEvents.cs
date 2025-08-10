using CesiPlayground.Shared.Events.Interfaces;


namespace CesiPlayground.Client.SceneManagement
{
    /// <summary>
    /// Fired on the CLIENT BUS to request a transition to a new scene.
    /// Listened to by the ClientSceneFlowController.
    /// </summary>
    public struct TransitionToSceneEvent : IEvent
    {
        public DataScene SceneToLoad;
        public DataScene SceneToUnload;
        public string DestinationGateName;
    }
}