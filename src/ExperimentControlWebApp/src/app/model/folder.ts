import { Experiment } from './experiment';

export class Folder {
  name?: string;
  subFolders?: Folder[];
  experiments?: Experiment[];
}
