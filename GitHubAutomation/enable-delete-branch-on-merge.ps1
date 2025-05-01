param($Org = '51Degrees')
$list = gh repo list 51Degrees --no-archived --limit 256 --json name --jq '.[].name'

Write-Host -Separator "`n" $list
Write-Host '---------'
$confirm = Read-Host 'The repositories above will have the "Delete branch on merge" setting enabled. Continue [y/N]'
if ($confirm -like 'y*') {
    foreach ($repo in $list) {
        gh repo edit $Org/$repo --delete-branch-on-merge
    }
}
