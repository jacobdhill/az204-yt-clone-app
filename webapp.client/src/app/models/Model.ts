export interface Result<T> {
  value: T;
  statusCode: number;
  contentType: string;
}

export interface Video {
  id: string;
  title: string;
  description: string;
  tags: string[];
  sourceUrl: string;
  thumbnailUrl: string;
}
