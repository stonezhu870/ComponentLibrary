
using System;
using System.Drawing;
using System.Collections;


namespace NetFocus.Components.TextEditor.Document
{
	public enum FoldType {
		Unspecified,
		MemberBody,
		Region,
		TypeBody
	}
	
	public class FoldMarker : AbstractSegment, IComparable
	{
		bool      isFolded = false;
		string    foldText = "...";
		FoldType  foldType = FoldType.Unspecified;
		IDocument document = null;

		public bool IsFolded 
		{
			get 
			{
				return isFolded;
			}
			set 
			{
				isFolded = value;
			}
		}
		
		public string FoldText 
		{
			get 
			{
				return foldText;
			}
		}
		
		public FoldType FoldType 
		{
			get {
				return foldType;
			}
			set {
				foldType = value;
			}
		}
		

		public int StartLine {
			get {
				if (offset > document.TextLength) {
					return -1;
				}
				return document.GetLineNumberForOffset(offset);
			}
		}
		
		public int StartColumn {
			get {
				if (offset > document.TextLength) {
					return -1;
				}
				return offset - document.GetLineSegmentForOffset(offset).Offset;
			}
		}
		
		public int EndLine {
			get {
				if (offset + length > document.TextLength) {
					return document.TotalNumberOfLines + 1;
				}
				return document.GetLineNumberForOffset(offset + length);
			}
		}
		public int EndColumn {
			get {
				if (offset + length > document.TextLength) {
					return -1;
				}
				return offset + length - document.GetLineSegmentForOffset(offset + length).Offset;
			}
		}
		

		public string InnerText {
			get {
				return document.GetText(offset, length);
			}
		}
		
		//定义5个重载的构造函数,只有参数最多的一个真正实现初始化.
		public FoldMarker(IDocument document, int offset, int length, string foldText, bool isFolded)
		{
			this.document = document;
			this.offset   = offset;
			this.length   = length;
			this.foldText = foldText;
			this.isFolded = isFolded;
		}
		
		public FoldMarker(IDocument document, int startLine, int startColumn, int endLine, int endColumn) : this(document, startLine, startColumn, endLine, endColumn, FoldType.Unspecified)
		{
		}
		
		public FoldMarker(IDocument document, int startLine, int startColumn, int endLine, int endColumn, FoldType foldType)  : this(document, startLine, startColumn, endLine, endColumn, foldType, "...")
		{
		}
		
		public FoldMarker(IDocument document, int startLine, int startColumn, int endLine, int endColumn, FoldType foldType, string foldText) : this(document, startLine, startColumn, endLine, endColumn, foldType, foldText, false)
		{
		}
		
		public FoldMarker(IDocument document, int startLine, int startColumn, int endLine, int endColumn, FoldType foldType, string foldText, bool isFolded)
		{
			this.document = document;
			
			startLine = Math.Min(document.TotalNumberOfLines - 1, Math.Max(startLine, 0));
			ISegment startLineSegment = document.GetLineSegment(startLine);
			
			endLine = Math.Min(document.TotalNumberOfLines - 1, Math.Max(endLine, 0));
			ISegment endLineSegment   = document.GetLineSegment(endLine);
			
			this.FoldType = foldType;
			this.foldText = foldText;
			this.offset = startLineSegment.Offset + startColumn;
			this.length = (endLineSegment.Offset + endColumn) - this.offset;
			this.isFolded = isFolded;
		}
		
		
		//implement the IComparable interface.
		public int CompareTo(object o)
		{
			if (!(o is FoldMarker)) {
				throw new ArgumentException();
			}
			FoldMarker f = (FoldMarker)o;
			if (offset != f.offset) {
				return offset.CompareTo(f.offset);
			}
			
			return length.CompareTo(f.length);
		}
	}
}
