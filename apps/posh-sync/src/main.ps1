function Push($filePath, $storePath, $namespace) {
    $nameInStore = $namespace, (Split-Path $filePath -Leaf) -join '.'
    $targetPath = $storePath | Join-Path -ChildPath $nameInStore

    Get-Content $filePath -Raw |
        ConvertFrom-Json |
        ConvertTo-Json -Compress -Depth 100 |
        Set-Content -Path $targetPath -Encoding utf8NoBOM
}

$root = $PSCommandPath |
    Split-Path -Parent |
    Split-Path -Parent

$params = @{
    filePath  = $root |
        Join-Path -ChildPath 'external' |
        Join-Path -ChildPath 'mock-config.json'
    storePath = $root | Join-Path -ChildPath 'store'
    namespace = 'develop.mock.config'
}

Push @params
