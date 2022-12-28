using Configs;
using Server;
using Zenject;

namespace GameModels
{
    public interface IGameController
    {
        void Jump();
        void FirstJump();
        void BonusJump();
        void Cashout();
    }

    public class GameController : IGameController
    {
        [Inject] private IGameStorage _gameStorage;
        [Inject] private IAccountModel _accountModel;
        [Inject] private IJumperServerApi _jumperServerApi;
        [Inject] private IGameConfigs _gameConfigs;

        public void Jump()
        {
            _jumperServerApi.Jump();
        }

        public void FirstJump()
        {
            _jumperServerApi.ToBet(new BetRequest(_gameStorage.BetAmount, _accountModel.CurrentCurrency, _gameStorage.IsWithBonus));
            _accountModel.ChangeBalance(-_gameStorage.BetAmount);
        }

        public void BonusJump()
        {
            _gameStorage.SetBonusStart(true);
            _jumperServerApi.ToBet(new BetRequest(_gameStorage.BetAmount, _accountModel.CurrentCurrency, _gameStorage.IsWithBonus));
            _accountModel.ChangeBalance(-_gameStorage.BetAmount * _gameConfigs.BonusFactor);
        }
        
        public void Cashout()
        {
            _jumperServerApi.Cashout();
        }
    }
}