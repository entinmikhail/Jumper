using Server;
using Zenject;

namespace GameModels
{
    public interface IGameController
    {
        void Jump();
        void FirstJump(float bet);
        void ActivateBonusJump();
        void Cashout();
    }

    public class GameController : IGameController
    {
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IAccountModel _accountModel;
        [Inject] private IJumperServerApi _jumperServerApi;

        public void Jump()
        {
            _jumperServerApi.Jump();
        }

        public void FirstJump(float bet)
        {
            _jumperServerApi.ToBet(new BetRequest(bet, _accountModel.CurrentCurrency, _gameStorage.IsWithBonus));
        }
        
        public void ActivateBonusJump()
        {
            _gameStorage.SetBonusStart(true);
        }

        public void Cashout()
        {
            _jumperServerApi.Cashout();
        }
    }
}