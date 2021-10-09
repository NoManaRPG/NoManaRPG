// This file is part of the WafclastRPG project.

using System;
using System.Text;
using WafclastRPG.Game.Entities.Monsters;
using WafclastRPG.Game.Entities.Skills;
using WafclastRPG.Game.Entities.Skills.Effects;
using WafclastRPG.Game.Extensions;

namespace WafclastRPG.Game.Entities
{
    public class WafclastCombat
    {
        private readonly WafclastPlayer _player;
        private readonly WafclastMonster _monster;
        private readonly Random _rd;

        public WafclastCombat(WafclastPlayer player, WafclastMonster monster)
        {
            this._player = player;
            this._monster = monster;
            this._rd = new Random();
        }

        public (bool ContinueCombat, StringBuilder CombatDescription) PlayerUseSkill(WafclastBaseSkill skill)
        {
            var str = new StringBuilder();
            double damage;
            bool roundEnd = false;

            str.AppendLine($"{this._player.Mention} efetuou <:weapon:896025534650253373> **{skill.Name}** em **{this._monster.NameLevel}**!");
            foreach (var item in skill.Effects)
            {
                switch (item)
                {
                    case WafclastSkillDamageEffect de:
                        if (Mathematics.CalculateHitChance(this._player.Character.PrecisionPoints, this._monster.EvasionPoints))
                        {
                            damage = this._rd.Sortear(this._player.Character.Damage + de.Damage);
                            roundEnd = this._monster.TakeDamage(damage);
                            str.AppendLine($"Causou {this._player.Character.EmojiAttack} **{damage:N2}**  de dano!");
                        }
                        else
                            str.AppendLine($"{this._monster.NameLevel} desviou!");
                        break;
                }
            }

            str.AppendLine();
            if (roundEnd)
            {
                str.AppendLine($"{Emojis.CrossBone} **{this._monster.NameLevel}** morreu!");
                str.AppendLine();
            }
            return (!roundEnd, str);
        }

        public (bool ContinueCombat, StringBuilder CombatDescription) MonsterUseSkill(WafclastBaseSkill skill)
        {
            var str = new StringBuilder();
            double damage;
            bool roundEnd = false;


            str.AppendLine($"{_player.Mention} efetuou <:weapon:896025534650253373> **{skill.Name}** em **{_monster.NameLevel}**!");
            foreach (var item in skill.Effects)
            {
                switch (item)
                {
                    case WafclastSkillDamageEffect de:
                        if (Mathematics.CalculateHitChance(this._player.Character.PrecisionPoints, this._monster.EvasionPoints))
                        {
                            damage = this._rd.Sortear(this._player.Character.Damage + de.Damage);
                            roundEnd = this._monster.TakeDamage(damage);
                            str.AppendLine($"Causou {this._player.Character.EmojiAttack} **{damage:N2}**  de dano!");
                        }
                        else
                            str.AppendLine($"{this._monster.NameLevel} desviou!");
                        break;
                }
            }

            str.AppendLine();
            if (roundEnd)
            {
                str.AppendLine($"{Emojis.CrossBone} **{this._monster.NameLevel}** morreu!");
                str.AppendLine();
            }
            return (!roundEnd, str);
        }

        //if (attacking.isMonster)
        //{
        //    if (Mathematics.CalculateHitChance(monster.PrecisionPoints, this.Character.EvasionPoints))
        //    {
        //        damage = this.Character.ReceiveDamage(rd.Sortear(monster.Damage));
        //        str.AppendLine($"{this.Mention} recebeu {damage:N2} de dano!");

        //        if (this.Character.IsDead)
        //        {
        //            str.AppendLine($"{this.Mention} {Emojis.CrossBone} morreu! ");
        //            str.AppendLine($"{this.Mention} perdeu nível!");
        //            this.Character.Deaths++;
        //            this.Character.RemoveOneLevel();
        //            return str.ToString();
        //        }
        //    }
        //    else
        //    {
        //        str.AppendLine($"{this.Mention} desviou!");
        //    }
        //}
        //if (attacking.isPlayer)
        //{
        //    if (Mathematics.CalculateHitChance(this.Character.PrecisionPoints, monster.EvasionPoints))
        //    {
        //        damage = monster.TakeDamage(rd.Sortear(this.Character.Damage));
        //        str.AppendLine($"{monster.Mention} recebeu {damage:N2} {this.Character.EmojiAttack} de dano!");

        //        if (monster.LifePoints.Current <= 0)
        //        {
        //            str.AppendLine($"{monster.Mention} {Emojis.CrossBone} morreu!");
        //            this.Character.MonsterKills++;
        //            this.Character.AddExperience(monster.Level * monster.Level);
        //        }
        //    }
        //    else
        //    {
        //        str.AppendLine($"{monster.Mention} desviou!");
        //    }
        //}

        //return str.ToString();
    }
}
