public interface IHealthHandler
{
    public void ReceiveDamage(int pAmount);

    public void DetermineBuffResult(BuffData pData);

    public void GetInstantHealth(BuffData pData);

    public void GetHealthRegen(BuffData pData);
}