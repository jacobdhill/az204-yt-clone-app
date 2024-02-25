export interface ReadVideoDto {
  id: string;
  title: string;
  description: string;
  tags: string[];
  sourceUrl: string;
  createdDateUtc: Date;
}

export interface ListVideoDto {
  id: string;
  title: string;
  description: string;
  tags: string[];
  thumbnailUrl: string;
}

export interface ListCommentDto {
  id: string;
  message: string;
  likeCount: number;
  createdDateUtc: Date;
}
