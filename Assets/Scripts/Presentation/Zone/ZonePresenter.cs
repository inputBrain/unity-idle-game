using Model.Zone;

namespace Presentation.Zone
{
    public class ZonePresenter
    {
        private ZoneModel _zoneModel;
        private ZoneView _zoneView;


        public void Init(ZoneModel zoneModel, ZoneView zoneView)
        {
            _zoneView = zoneView;
            _zoneModel = zoneModel;

            _zoneModel.CurrentZone.OnValueChanged += _zoneView.UpdateZoneText;
        }
        
        
        public  void IncreaseZone()
        {
            _zoneModel.CurrentZone.Value++;
        }        
        
        public  void DecreaseZone()
        {
            if (_zoneModel.CurrentZone.Value <= 1)
            {
                return;
            }
            _zoneModel.CurrentZone.Value--;
        }
    }
}