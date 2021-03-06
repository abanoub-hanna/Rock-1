-- NOTE: if you have SSMS 2012 or newer consider turning on Retain CR/LF on copy or save. This will allow you to Copy and Paste directly from the Grid results
-- Options > Query Results > Sql Server > Results to Grid > 'Retain CR/LF on copy or save'

DECLARE @BinaryFileIds TABLE (Id INT NOT NULL)

INSERT INTO @BinaryFileIds
SELECT Id
FROM BinaryFile
WHERE Id IN ( -- ##TODO##: change this to indicate which binary file records to create a migration for
		SELECT ct.ImageFileId
		FROM CommunicationTemplate ct
		)

DECLARE @crlf VARCHAR(2) = CHAR(13) + CHAR(10)
DECLARE @codeGen VARCHAR(max) = 'DECLARE
	@BinaryFileId int
	,@BinaryFileTypeIdDefault int = (SELECT TOP 1 Id from [BinaryFileType] where [Guid] = ''C1142570-8CD6-4A20-83B1-ACB47C1CD377'')
	,@StorageEntityTypeIdDatabase int = (SELECT TOP 1 Id FROM [EntityType] WHERE [Guid] = ''0AA42802-04FD-4AEC-B011-FEB127FC85CD'')
';

SELECT @codeGen += CONCAT (
		'
-- Add ' + b.[FileName] + '
IF NOT EXISTS (SELECT * FROM [BinaryFile] WHERE [Guid] = ''' + convert(VARCHAR(max), b.[Guid]) + ''' )
BEGIN
INSERT INTO [BinaryFile] ([IsTemporary], [IsSystem], [BinaryFileTypeId], [FileName], [MimeType], [StorageEntityTypeId], [Guid])
			VALUES (0,0, @BinaryFileTypeIdDefault, ''' + b.[FileName] + ''', ''' + b.[MimeType] + ''', @StorageEntityTypeIdDatabase, ''' + convert(VARCHAR(max), b.[Guid]) + ''')

SET @BinaryFileId = SCOPE_IDENTITY()
'
		,'
INSERT INTO [BinaryFileData] ([Id], [Content], [Guid])
  VALUES ( 
    @BinaryFileId
    ,'
		,'0x' + convert(VARCHAR(max), [Content], 2)
		,@crlf
		,'    ,'''
		,bfd.[Guid]
		,''''
		,@crlf
		,'    )
END
')
FROM [BinaryFileData] bfd
INNER JOIN [BinaryFile] b ON bfd.Id = b.id
WHERE b.Id IN (
		SELECT Id
		FROM @BinaryFileIds
		)

-- output as unescaped XML so that it doesn't get truncated
SELECT 1 as tag,
   null as parent,
   @crlf + '##BEGIN_CODEGEN##' + @crlf + @crlf 
   + @codeGen 
   + @crlf + @crlf + '###END_COPYGEN##' + @crlf [str!1!!cdata]
 for xml explicit