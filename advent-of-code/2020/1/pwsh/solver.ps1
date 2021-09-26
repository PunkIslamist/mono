# Just calling normal functions introduces insane performance penalties
# See: https://github.com/PowerShell/PowerShell/issues/8482
class funcs {
    static [int] sum($numbers) {
        $s = 0
        foreach ($n in $numbers) { $s += $n }
        return $s
    }
}

$numbers = $PSScriptRoot |
    Split-Path -Parent |
    Join-Path -ChildPath input |
    Join-Path -ChildPath expenses.txt |
    foreach { Get-Content -Path $_ } |
    foreach { [int]$_ } |
    sort

$numbers |
    sort -Descending |
    foreach {
        $x = $_
        $numbers | foreach { , ($x, $_) } |
            where { [funcs]::sum($_) -lt 2020 } |
            foreach {
                $xy = $_
                $numbers | foreach { , ( $xy + $_) } |
                    where { [funcs]::sum($_) -ge 2020 } |
                    select -First 1
                }
            } | 
            where { [funcs]::sum($_) -eq 2020 } |
            select -First 1
