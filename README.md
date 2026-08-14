# 📦 DNT-SDK-Mirror

> آینه‌‌ای غیررسمی از آخرین نسخه‌های SDK دات‌نت (NET SDKs) – به‌روزرسانی خودکار روزانه


آخرین نگارش SDK دات‌نت را می‌توانید از پوشه‌ی [sdks](/sdks) دریافت کنید.
این مخزن هر روز به صورت خودکار تغییرات رسمی را بررسی کرده و در صورت وجود، فایل‌های جدید را دریافت و در مخزن قرار می‌دهد.


---

## ⚠️ نکته مهم – فایل‌های چندبخشی


به دلیل محدودیت گیت‌هاب در حجم فایل‌های آپلودی، فایل‌های SDK به صورت **چندبخشی (split)** ذخیره می‌شوند.
فرمت فایل‌ها شبیه به نمونه‌ی زیر است:


```
archive.zip
archive.z01
archive.z02
...
```


تمامی برنامه‌های رایج فشرده‌سازی مانند **WinRAR**، **7-Zip**، **WinZip** و **PeaZip** در ویندوز قادر به باز کردن و ادغام خودکار این فایل‌ها هستند.


---

## 🧩 آموزش ادغام فایل‌ها با 7‑Zip (رایگان و متن‌باز)


**روش گرافیکی:**


1. تمام قطعات فایل (مانند `archive.z01`، `archive.z02` و `archive.zip`) را در **یک پوشه** قرار دهید.
2. روی فایل `archive.zip` (یا `archive.z01`) **کلیک راست** کنید.
3. از منوی `7-Zip` گزینه‌ی **`Extract Here`** یا **`Extract to "archive\"`** را انتخاب کنید.
4. ابزار به صورت خودکار تمام قطعات را شناسایی کرده و فایل اصلی را خارج می‌سازد.


برای **ادغام قطعات و تولید مجدد یک فایل ZIP واحد** نیز می‌توانید از مسیر <code>File → Merge Files</code> در نرم‌افزار 7‑Zip استفاده کنید.


---

## 💻 روش خط فرمان (مناسب برای سرور یا اسکریپت‌ها)


اگر به محیط غیرگرافیکی (مانند سرور لینوکسی یا خط فرمان ویندوز) دسترسی دارید، کافی است **7-Zip** نصب باشد و دستور زیر را در همان پوشه‌ای که قطعات قرار دارند اجرا کنید:


```bash
7z x archive.zip
```


✔️ این دستور به طور خودکار همه‌ی قطعات را پیدا کرده و فایل نهایی را استخراج می‌کند.


---

## 🔄 به‌روزرسانی خودکار


این مخزن هر روز از طریق **GitHub Actions** بررسی می‌شود. در صورتی که نسخه‌ی جدیدی از SDK دات‌نت منتشر شود، فایل‌های مربوطه بلافاصله دریافت، قطعه‌بندی و به شاخه‌ی <code>sdks/</code> اضافه می‌شوند.


---

## 📜 مجوز و نکات قانونی


این پروژه به صورت غیررسمی و تنها برای سهولت دسترسی به SDKهای دات‌نت ایجاد شده است. تمامی حقوق متعلق به **Microsoft Corporation** می‌باشد.


---

## 📦 لیست فایل‌های دریافتی


<!---->

**کانال دات‌نت 8.0:**
|فایل|حجم|تاریخ ارائه|
|---|---|---|
|[windowsdesktop-runtime-8.0.30-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/windowsdesktop-runtime-8.0.30-win-x64.zip)|11.1 MB|1405/05/20|
|[windowsdesktop-runtime-8.0.30-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/windowsdesktop-runtime-8.0.30-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-sdk-8.0.424-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-sdk-8.0.424-win-x64.zip)|37.2 MB|1405/05/20|
|[dotnet-sdk-8.0.424-win-x64.z04](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-sdk-8.0.424-win-x64.z04)|47.2 MB|1405/05/20|
|[dotnet-sdk-8.0.424-win-x64.z03](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-sdk-8.0.424-win-x64.z03)|47.2 MB|1405/05/20|
|[dotnet-sdk-8.0.424-win-x64.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-sdk-8.0.424-win-x64.z02)|47.2 MB|1405/05/20|
|[dotnet-sdk-8.0.424-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-sdk-8.0.424-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-runtime-8.0.30-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-runtime-8.0.30-win-x64.zip)|28.3 MB|1405/05/20|
|[dotnet-hosting-8.0.30-win.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-hosting-8.0.30-win.zip)|17.1 MB|1405/05/20|
|[dotnet-hosting-8.0.30-win.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-hosting-8.0.30-win.z02)|47.2 MB|1405/05/20|
|[dotnet-hosting-8.0.30-win.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/dotnet-hosting-8.0.30-win.z01)|47.2 MB|1405/05/20|
|[aspnetcore-runtime-8.0.30-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/8.0/aspnetcore-runtime-8.0.30-win-x64.zip)|10.3 MB|1405/05/20|




**کانال دات‌نت 9.0:**
|فایل|حجم|تاریخ ارائه|
|---|---|---|
|[windowsdesktop-runtime-9.0.19-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/windowsdesktop-runtime-9.0.19-win-x64.zip)|13 MB|1405/05/20|
|[windowsdesktop-runtime-9.0.19-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/windowsdesktop-runtime-9.0.19-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-sdk-9.0.317-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-sdk-9.0.317-win-x64.zip)|36.6 MB|1405/05/20|
|[dotnet-sdk-9.0.317-win-x64.z04](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-sdk-9.0.317-win-x64.z04)|47.2 MB|1405/05/20|
|[dotnet-sdk-9.0.317-win-x64.z03](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-sdk-9.0.317-win-x64.z03)|47.2 MB|1405/05/20|
|[dotnet-sdk-9.0.317-win-x64.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-sdk-9.0.317-win-x64.z02)|47.2 MB|1405/05/20|
|[dotnet-sdk-9.0.317-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-sdk-9.0.317-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-runtime-9.0.19-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-runtime-9.0.19-win-x64.zip)|29.5 MB|1405/05/20|
|[dotnet-hosting-9.0.19-win.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-hosting-9.0.19-win.zip)|20.7 MB|1405/05/20|
|[dotnet-hosting-9.0.19-win.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-hosting-9.0.19-win.z02)|47.2 MB|1405/05/20|
|[dotnet-hosting-9.0.19-win.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/dotnet-hosting-9.0.19-win.z01)|47.2 MB|1405/05/20|
|[aspnetcore-runtime-9.0.19-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/9.0/aspnetcore-runtime-9.0.19-win-x64.zip)|10.4 MB|1405/05/20|




**کانال دات‌نت 10.0:**
|فایل|حجم|تاریخ ارائه|
|---|---|---|
|[windowsdesktop-runtime-10.0.11-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/windowsdesktop-runtime-10.0.11-win-x64.zip)|12.4 MB|1405/05/20|
|[windowsdesktop-runtime-10.0.11-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/windowsdesktop-runtime-10.0.11-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-sdk-10.0.400-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-sdk-10.0.400-win-x64.zip)|26.3 MB|1405/05/20|
|[dotnet-sdk-10.0.400-win-x64.z04](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-sdk-10.0.400-win-x64.z04)|47.2 MB|1405/05/20|
|[dotnet-sdk-10.0.400-win-x64.z03](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-sdk-10.0.400-win-x64.z03)|47.2 MB|1405/05/20|
|[dotnet-sdk-10.0.400-win-x64.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-sdk-10.0.400-win-x64.z02)|47.2 MB|1405/05/20|
|[dotnet-sdk-10.0.400-win-x64.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-sdk-10.0.400-win-x64.z01)|47.2 MB|1405/05/20|
|[dotnet-runtime-10.0.11-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-runtime-10.0.11-win-x64.zip)|30.1 MB|1405/05/20|
|[dotnet-hosting-10.0.11-win.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-hosting-10.0.11-win.zip)|22.5 MB|1405/05/20|
|[dotnet-hosting-10.0.11-win.z02](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-hosting-10.0.11-win.z02)|47.2 MB|1405/05/20|
|[dotnet-hosting-10.0.11-win.z01](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/dotnet-hosting-10.0.11-win.z01)|47.2 MB|1405/05/20|
|[aspnetcore-runtime-10.0.11-win-x64.zip](https://github.com/VahidN/DntSdkMirror/raw/refs/heads/main/sdks/10.0/aspnetcore-runtime-10.0.11-win-x64.zip)|10.8 MB|1405/05/20|



