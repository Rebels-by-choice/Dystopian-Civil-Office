export interface CreateDocumentModel {
  name: string;
  category: string;
  importDate: string;
}

export interface UpdateDocumentModel {
  name?: string;
  category?: string;
}

export interface DeleteDocumentModel {
  documentId: number;
}
