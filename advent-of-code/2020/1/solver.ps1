$numbers = $PSScriptRoot |
    Join-Path -ChildPath .\expenses.txt |
    foreach { Get-Content -Path $_ } |
    foreach { [int]$_ } |
    sort

$numbers |
    sort -Descending |
    foreach {
        $x = $_
        $numbers |
            foreach { @{x = $x; y = $_ } } |
            where { $_.x + $_.y -ge 2020 } |
            select -First 1
        } | 
        where { $_.x + $_.y -eq 2020 } |
        select -First 1 |
        foreach { $_.x * $_.y }
