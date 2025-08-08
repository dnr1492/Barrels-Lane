using UnityEditor;
using UnityEngine;

public static class SetSpriteBorderTool
{
    //L=5, T=5, R=5, B=4 (TextureImporter�� L,B,R,T ����)
    private static readonly Vector4 Border = new Vector4(5, 4, 5, 5);

    [MenuItem("�����/Sprite/[��������Ʈ�� ������ ���¿��� Ŭ��] Set Border L5 T5 R5 B4")]
    private static void SetBorder()
    {
        foreach (var obj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null || importer.textureType != TextureImporterType.Sprite)
                continue;

            //���� ��������Ʈ
            if (importer.spriteImportMode == SpriteImportMode.Single)
            {
                importer.spriteBorder = Border;
                ////Pivot�� ���� �����ϰ� ������ �ּ� ����
                //importer.spriteAlignment = (int)SpriteAlignment.Center;
                //importer.spritePivot = new Vector2(0.5f, 0.5f);
            }
            //��Ƽ(��������Ʈ ��Ʈ)
            else if (importer.spriteImportMode == SpriteImportMode.Multiple)
            {
                var metas = importer.spritesheet;
                for (int i = 0; i < metas.Length; i++)
                {
                    metas[i].border = Border;
                    ////Pivot�� ���� �����ϰ� ������:
                    //metas[i].alignment = (int)SpriteAlignment.Center;
                    //metas[i].pivot = new Vector2(0.5f, 0.5f);
                }
                importer.spritesheet = metas;
            }

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
        Debug.Log("SetSpriteBorder: �Ϸ� (L=5, T=5, R=5, B=4)");
    }
}
