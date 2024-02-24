export interface VideoReadDto {
  id: string;
  title: string;
  description: string;
  tags: string[];
  sourceUrl: string;
  createdDateUtc: Date;
}

export interface VideoListDto {
  id: string;
  title: string;
  description: string;
  tags: string[];
  thumbnailUrl: string;
}
