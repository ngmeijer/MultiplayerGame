public interface IHealthHandler
{
    public void ReceiveDamage(int pAmount);

    public void DetermineBuffResult(BuffData pData);

    public void GetInstantHealth(int pAmount);

    public void GetHealthRegen(int pAmount, float pTime, float pInterval);
}