public abstract class CharacterState
{
    protected CharacterAI character;

    public CharacterState(CharacterAI character)
    {
        this.character = character;
    }
    
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}