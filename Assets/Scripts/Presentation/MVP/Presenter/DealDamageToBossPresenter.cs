using Domain.Entities;

namespace Presentation.MVP.Presenter
{
    public class DealDamageToBossPresenter
    {
        private readonly Boss _boss;


        public DealDamageToBossPresenter(Boss boss)
        {
            _boss = boss;
        }
        
        
    }
}