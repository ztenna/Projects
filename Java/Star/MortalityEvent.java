// Zachary Tennant

class MortalityEvent
{
	static final int DEATH = 1;
	LivingThings mortal;
	int vitality;

	public MortalityEvent(LivingThings mortal, int vitality)
	{
		this.mortal = mortal;
		this.vitality = vitality;
	}
}