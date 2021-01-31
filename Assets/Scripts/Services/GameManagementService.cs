using TicTacToe3D.Games;

namespace TicTacToe3D.Services
{
    public sealed class GameManagementService : ServiceBase
    {
        internal GameBase CurrentGame { get; set; }

        protected override void RegisterService() =>
            Bootstrap.BootstrapInstance.RegisterService(this);

        protected override void Awake()
        {
            base.Awake();
            CurrentGame = gameObject.AddComponent<TicTacToe3DGame>();
        }
    }
}