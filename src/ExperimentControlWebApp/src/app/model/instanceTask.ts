import { ClientResult } from './clientResult';
import { TaskSet } from './taskSet';

export interface InstanceTask {
  set?: TaskSet;
  id?: number;
  name?: string;
  results?: Array<ClientResult>;
  inputData?: { [key: string]: string };

  running?: boolean;
  failed?: boolean;
  finished?: boolean;
}
