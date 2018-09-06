import { Dataset } from './dataset';
import { Experiment } from './experiment';
import { FixedParameter } from './parameters';
import { InstanceTask } from './instanceTask';

export interface TaskSet {
  experiment?: Experiment;
  parameters?: Array<FixedParameter>;
  id?: number;
  name?: string;
  inputDataset?: Dataset;
  values?: { [key: string]: number };
  tasks?: Array<InstanceTask>;

  running?: boolean;
  failed?: boolean;
  finished?: boolean;
}
