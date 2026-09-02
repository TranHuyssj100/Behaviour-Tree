using System.Collections.Generic;

public static class SaveMigration
{
    public static PlayerSaveDto Migrate(PlayerSaveDto save)
    {
        if (save == null)
            return new PlayerSaveDto();

        // Mỗi lần đổi format save, tăng PlayerSaveDto.CurrentVersion và thêm bước chuyển ở đây.
        if (save.version <= 0)
            save.version = PlayerSaveDto.CurrentVersion;

        save.items ??= new List<ItemStackDto>();
        save.shops ??= new List<ShopStateDto>();
        save.version = PlayerSaveDto.CurrentVersion;
        return save;
    }
}
