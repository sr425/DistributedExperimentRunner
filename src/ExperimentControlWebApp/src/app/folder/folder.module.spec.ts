import { FolderModule } from './folder.module';

describe('FolderModule', () => {
  let folderModule: FolderModule;

  beforeEach(() => {
    folderModule = new FolderModule();
  });

  it('should create an instance', () => {
    expect(folderModule).toBeTruthy();
  });
});
