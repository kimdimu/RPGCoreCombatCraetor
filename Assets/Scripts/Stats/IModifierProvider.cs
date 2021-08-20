using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifires(Stat stat); //여러 float들을 리턴하는 IEnumerator을 반환한다고 이해해도 괜찮을까
        IEnumerable<float> GetPercentageModifires(Stat stat); //여러 float들을 리턴하는 IEnumerator을 반환한다고 이해해도 괜찮을까
    } 
}
