namespace Presentation.Zone
{
    public class ZonePresenter
    {
        private Domain.Entities.Zone _zone;
        private ZoneView _zoneView;


        public void Init(Domain.Entities.Zone zone, ZoneView zoneView)
        {
            _zoneView = zoneView;
            _zone = zone;

            _zone.CurrentZone.OnValueChanged += _zoneView.UpdateZoneText;
        }
        
        
        public  void IncreaseZone()
        {
            _zone.CurrentZone.Value++;
        }        
        
        public  void DecreaseZone()
        {
            if (_zone.CurrentZone.Value <= 1)
            {
                return;
            }
            _zone.CurrentZone.Value--;
        }
    }
}