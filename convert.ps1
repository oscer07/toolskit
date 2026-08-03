Add-Type -AssemblyName System.Drawing
$img = [System.Drawing.Image]::FromFile("icon.jpg")
$img.Save("icon.png", [System.Drawing.Imaging.ImageFormat]::Png)
