using System;

public class Stat
{
    public int Value { get; set; }
    public int MaxValue { get; set; }
}

[Serializable]
public class Attribute
{
    public int Str; // 힘
    public int Int; // 지능
    public int Vit; // 방어력
    public int Agi; // 스피드
    public int Luk; // 행운

    public void Initialize(int str, int intelligence, int vit, int agi, int luk)
    {
        Str = str;
        Int = intelligence;
        Vit = vit;
        Agi = agi;
        Luk = luk;
    }
}