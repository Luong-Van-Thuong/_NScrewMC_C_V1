# CLAUDE.md

Hướng dẫn cho Claude Code khi làm việc với repository này.

## Tổng quan

`SCREW_MC` — ứng dụng **WinForms (.NET Framework 4.7.2)** điều khiển máy bắt vít tự động (auto screw machine) trong dây chuyền lắp ráp công nghiệp. Phần mềm điều phối motion controller EtherCAT, camera vision, đọc barcode, giao tiếp PLC/thiết bị qua TCP & Modbus, và lưu dữ liệu bằng SQLite.

- **Solution:** `_NScrewMC_C_V1.sln` → project chính `_NScrewMC_C_V1\_NScrewMC_C_V1.csproj`
- **AssemblyName:** `SCREW_MC` · **RootNamespace:** `_NScrewMC_C_V1`
- **Entry point:** `Program.Main` (`_NScrewMC_C_V1\Program.cs`) → khởi tạo motion controller (`Connect_MMC`), chặn chạy 2 lần bằng `Mutex "SCREW_MC"`, rồi `Application.Run(new FormMain())`.
- **Form chính:** `FormMain` định nghĩa trong `Form\Main Form\ID_DLG_MAIN.cs`.
- **Version string:** sửa thủ công ở `Program.Version` (lịch sử thay đổi ghi dạng comment ngay trên đó).

## Build & Run

Project là .NET Framework + WinForms + COM/ActiveX nên **phải build bằng MSBuild của Visual Studio**, KHÔNG dùng `dotnet build` (SDK .NET không build được designer/ActiveX của Framework).

MSBuild có sẵn trên máy này:
```
C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe
```

Build (lưu ý platform của solution là `"Any CPU"` có dấu cách — build trực tiếp csproj với `AnyCPU` thì gọn hơn):
```powershell
$msb = "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe"
& $msb "_NScrewMC_C_V1\_NScrewMC_C_V1.csproj" /t:Build /p:Configuration=Debug /p:Platform=AnyCPU /v:minimal
```

- **OutputPath (Debug|AnyCPU và x64):** `C:\FA\SCREW-C-V7.0\PGM\` — app được thiết kế chạy từ thư mục này (deployment thực tế). Đây là chủ đích, không phải lỗi; chỉ đổi nếu có yêu cầu rõ ràng.
- NuGet packages nằm ở `packages\` (đã restore sẵn theo `packages.config`). Nếu thiếu, restore qua VS hoặc `msbuild /t:restore`.
- **Chạy:** `SCREW_MC.exe` phụ thuộc phần cứng thật (motion controller NMC/MMC, camera) và **ActiveX OCX `BTNENHLib4` phải được register** (`regsvr32`) trên máy thì UI mới load. Đây là yêu cầu runtime/deploy, không liên quan tới build.

### ⚠️ Cạm bẫy build quan trọng nhất: "Mark of the Web" (MSB3821)

Repo này thường được copy/giải nén từ file nén nên Windows gắn cờ **Zone.Identifier (mark-of-the-web)** lên file. MSBuild sẽ **từ chối xử lý `.resx`** với lỗi `MSB3821: Couldn't process file ... due to its being in the Internet or Restricted zone`. Đây là lỗi build hay gặp nhất ở đây.

Khắc phục — gỡ mark-of-the-web cho toàn bộ cây thư mục:
```powershell
Get-ChildItem "D:\Projects_\Cong_Ty\PControll\DaAll" -Recurse -File | Unblock-File
```
Chạy lại sau mỗi lần lấy code mới từ nguồn nén/tải về.

## Kiến trúc thư mục (trong `_NScrewMC_C_V1\`)

| Thư mục | Vai trò |
|---|---|
| `Program.cs` | Entry point, init motion controller, single-instance mutex. |
| `Form\Main Form\` | Các màn hình chính: `ID_DLG_MAIN` (FormMain), `ID_DLG_AUTO`, `ID_DLG_DATA`, `ID_DLG_TEACH`, `ID_DLG_SCREW_TEACH`, `ID_DLG_LOG`, `ID_DLG_TITLE`, `ID_DLG_BOTTOM`. |
| `Form\Sub Form\` | Dialog phụ: setting, teaching, alarm, IO monitor, keyboard ảo, camera control, v.v. (kiểu đặt tên `ID_DLG_*`). |
| `Thread\1_Base\` | Luồng nền nền tảng: `MTrsAutoManager` (điều phối auto), `MTrLog`/`MTrsLogGEIM`, `MTrLamp`, `MTrsBuzzer`, `MTrsOP`. |
| `Thread\2_Work\` | Luồng nghiệp vụ: `MTrsJig`, `MTrsScrew` (logic bắt vít theo step). |
| `Thread\3_Port\` | Luồng giao tiếp cổng: `Barcode`, `MyModbus`, `TPJog`. |
| `Device\Servo\` | Driver servo/motion: `NMCMotionSDK`, `MMCEtherCATAxis`, `AxisManager`. |
| `Device\Camera\` | Camera vision: `BaslerCam`, `VisionInterface`, `ICameraInterface`. |
| `Device\TCP\` | Mạng: `TCP_Server`, `ItemClient`, `NetWorkInterface`, `UtilProtocol`. |
| `Data\Variable\` | Model dữ liệu: `InforManager`, `InforProduct`, `InforServoParam`, `InforTeaching`, `InforModelImport`, `IniFile`. |
| `Data\Define Config\` | File cấu hình runtime: `Config.ini`, `ErrorMessage.ini`. |
| `System Util\` | Tiện ích lõi: `MSystem` (trạng thái máy + helper hiển thị ActiveX), `Define` (hằng số/enum), `DataManager`, `DataReadWrite`, `Alarm`, `SQLYJ`, `SystemIO`, `MyTool`, `CtrListView*`. |
| `Lib\` | DLL bên thứ ba (đường dẫn tương đối): Basler.Pylon, Cognex.DataMan.*, NModbus4, SUserControls, AxInterop/Interop.BTNENHLib4, EEIP, Spire.* ... |

## Quy ước trong code

- Đặt tên kiểu **MFC / Win32**: form & dialog `ID_DLG_*` / `IDC_*`; biến thành viên `m_` (vd `m_btCurrentPos`); control nút là ActiveX `AxBTNENHLib4.AxBtnEnh` (không phải `Button` của WinForms).
- Logic máy chạy theo **state machine dạng step** trong các `MTrs*` (luồng nền), tương tác với `MSystem` giữ trạng thái IO/sản phẩm.
- Comment kèm tag tác giả/ngày (vd `// add 250705 jlyoon`) — giữ nguyên phong cách này khi sửa.
- Code gốc dùng tiếng Hàn ở một số chuỗi/tên file ảnh — bình thường, không cần đổi.

## Reference & quy tắc khi sửa project

- **Luôn dùng đường dẫn tương đối** cho `<HintPath>` (trỏ vào `Lib\` hoặc `..\packages\`). KHÔNG hardcode đường dẫn máy cá nhân (vd `C:\Users\...\Desktop\`) — sẽ vỡ build trên máy khác.
- DLL bên thứ ba cần cho build phải nằm trong `Lib\` và được commit cùng repo.
- **KHÔNG commit** `bin\`, `obj\`, `packages\`, `*.user`, `.vs\` (đã được `.gitignore` loại trừ — giữ vậy).

## Vấn đề đã biết / cần dọn

- `_NScrewMC_C_V1\TabAutoScrewV21.csproj`: file project **cũ** (`TabAutoScrewV2_2Array_241206_2`, dùng OpenCvSharp4), trùng `ProjectGuid` với project chính, không nằm trong solution → nên xóa khỏi git để tránh nhầm lẫn.
- `_NScrewMC_C_V1\timeout.Restart();\`: thư mục rỗng tạo nhầm → nên xóa.
