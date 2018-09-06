export interface Dataset {
  id?: number;
  name?: string;
  description?: string;
  prefix?: string;
  files?: Array<{ [key: string]: string }>;
}
