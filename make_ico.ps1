Add-Type -AssemblyName System.Drawing
$bmp = [System.Drawing.Bitmap]::FromFile("icon.jpg")
$fs = [System.IO.File]::Create("icon.ico")
$bw = New-Object System.IO.BinaryWriter $fs

$bw.Write([byte]0); $bw.Write([byte]0)
$bw.Write([byte]1); $bw.Write([byte]0)
$bw.Write([byte]1); $bw.Write([byte]0)

$w = if ($bmp.Width -gt 255) { 0 } else { $bmp.Width }
$h = if ($bmp.Height -gt 255) { 0 } else { $bmp.Height }

$bw.Write([byte]$w); $bw.Write([byte]$h)
$bw.Write([byte]0); $bw.Write([byte]0)
$bw.Write([int16]1)
$bw.Write([int16]32)

$ms = New-Object System.IO.MemoryStream
$bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
$pngBytes = $ms.ToArray()

$bw.Write([int]$pngBytes.Length)
$bw.Write([int]22)
$bw.Write($pngBytes, 0, $pngBytes.Length)

$bw.Close()
$fs.Close()
$bmp.Dispose()
