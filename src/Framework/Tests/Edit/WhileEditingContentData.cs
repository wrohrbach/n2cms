using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using N2.Tests.Edit.Items;
using System.Web.UI.WebControls;
using System.Web.UI;
using N2.Web.UI.WebControls;
using Rhino.Mocks;
using System.Security.Principal;
using System.Reflection;
using N2.Definitions;

namespace N2.Tests.Edit
{
    [TestFixture]
    public class WhileEditingContentData : EditManagerTests
    {
        [Test]
        public void AddedEditors_AreReturned()
        {
            Type itemType = typeof(ComplexContainersItem);
            Control editorContainer = new Control();
            var added = editManager.AddEditors(definitions.GetDefinition(itemType), new ComplexContainersItem(), editorContainer, CreatePrincipal("someone"));
            Assert.AreEqual(5, added.Count());
            TypeAssert.Equals<TextBox>(added.Single(e => e.PropertyName == "MyProperty0").Control);
            TypeAssert.Equals<TextBox>(added.Single(e => e.PropertyName == "MyProperty1").Control);
            TypeAssert.Equals<TextBox>(added.Single(e => e.PropertyName == "MyProperty2").Control);
			TypeAssert.Equals<FreeTextArea>(added.Single(e => e.PropertyName == "MyProperty3").Control);
			TypeAssert.Equals<CheckBox>(added.Single(e => e.PropertyName == "MyProperty4").Control);

            WebControlAssert.Contains(typeof(FieldSet), editorContainer);
            WebControlAssert.Contains(typeof(TextBox), editorContainer);
            WebControlAssert.Contains(typeof(FreeTextArea), editorContainer);
            WebControlAssert.Contains(typeof(CheckBox), editorContainer);
        }

        [Test]
        public void CanUpdateEditors()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            var added = AddEditors(item);

            TextBox tbp0 = added.Single(c => c.PropertyName == "MyProperty0").Control as TextBox;
            TextBox tbp1 = added.Single(c => c.PropertyName == "MyProperty1").Control as TextBox;
            TextBox tbp2 = added.Single(c => c.PropertyName == "MyProperty2").Control as TextBox;
            FreeTextArea ftap3 = added.Single(c => c.PropertyName == "MyProperty3").Control as FreeTextArea;
			CheckBox cbp4 = added.Single(c => c.PropertyName == "MyProperty4").Control as CheckBox;

            Assert.IsEmpty(tbp0.Text);
            Assert.IsEmpty(tbp1.Text);
            Assert.IsEmpty(tbp2.Text);
            Assert.IsEmpty(ftap3.Text);
            Assert.IsFalse(cbp4.Checked);

            item.MyProperty0 = "one";
            item.MyProperty1 = "two";
            item.MyProperty2 = "three";
            item.MyProperty3 = "rock";
            item.MyProperty4 = true;

			editManager.UpdateEditors(definitions.GetDefinition(item.GetContentType()), item, added, CreatePrincipal("someone"));

            Assert.AreEqual("one", tbp0.Text);
            Assert.AreEqual("two", tbp1.Text);
            Assert.AreEqual("three", tbp2.Text);
            Assert.AreEqual("rock", ftap3.Text);
            Assert.IsTrue(cbp4.Checked);
        }

        [Test]
        public void CanUpdateItem()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            var added = AddEditors(item);

			TextBox tbp0 = added.Single(c => c.PropertyName == "MyProperty0").Control as TextBox;
			TextBox tbp1 = added.Single(c => c.PropertyName == "MyProperty1").Control as TextBox;
			TextBox tbp2 = added.Single(c => c.PropertyName == "MyProperty2").Control as TextBox;
			FreeTextArea ftap3 = added.Single(c => c.PropertyName == "MyProperty3").Control as FreeTextArea;
			CheckBox cbp4 = added.Single(c => c.PropertyName == "MyProperty4").Control as CheckBox;

            Assert.IsEmpty(item.MyProperty0);
            Assert.IsEmpty(item.MyProperty1);
            Assert.IsEmpty(item.MyProperty2);
            Assert.IsEmpty(item.MyProperty3);
            Assert.IsFalse(item.MyProperty4);

            tbp0.Text = "one";
            tbp1.Text = "two";
            tbp2.Text = "three";
            ftap3.Text = "rock";
            cbp4.Checked = true;

			editManager.UpdateItem(definitions.GetDefinition(item.GetContentType()), item, added, CreatePrincipal("someone"));

            Assert.AreEqual("one", item.MyProperty0);
            Assert.AreEqual("two", item.MyProperty1);
            Assert.AreEqual("three", item.MyProperty2);
            Assert.AreEqual("rock", item.MyProperty3);
            Assert.IsTrue(item.MyProperty4);
        }

        [Test]
        public void UpdateItem_WithChanges_ReturnsTrue()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            var added = AddEditors(item);

			TextBox tbp0 = added.Single(c => c.PropertyName == "MyProperty0").Control as TextBox;
			TextBox tbp1 = added.Single(c => c.PropertyName == "MyProperty1").Control as TextBox;
			TextBox tbp2 = added.Single(c => c.PropertyName == "MyProperty2").Control as TextBox;
			FreeTextArea ftap3 = added.Single(c => c.PropertyName == "MyProperty3").Control as FreeTextArea;
			CheckBox cbp4 = added.Single(c => c.PropertyName == "MyProperty4").Control as CheckBox;

            tbp0.Text = "one";
            tbp1.Text = "two";
            tbp2.Text = "three";
            ftap3.Text = "rock";
            cbp4.Checked = true;

			var result = editManager.UpdateItem(definitions.GetDefinition(item.GetContentType()), item, added, CreatePrincipal("someone"));

            Assert.That(result.Length, Is.GreaterThan(0), "UpdateItem didn't return true even though the editors were changed.");
        }

        [Test]
        public void UpdateItem_WithNoChanges_ReturnsFalse()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            Type itemType = item.GetContentType();
            Control editorContainer = new Control();
			var added = editManager.AddEditors(definitions.GetDefinition(itemType), item, editorContainer, CreatePrincipal("someone"));

            item.MyProperty0 = "one";
            item.MyProperty1 = "two";
            item.MyProperty2 = "three";
            item.MyProperty3 = "rock";
            item.MyProperty4 = true;
			editManager.UpdateEditors(definitions.GetDefinition(item.GetContentType()), item, added, CreatePrincipal("someone"));

			var result = editManager.UpdateItem(definitions.GetDefinition(item.GetContentType()), item, added, null);

            Assert.IsFalse(result.Length > 0, "UpdateItem didn't return false even though the editors were unchanged.");
        }

		[Test]
		[Obsolete]
        public void CanSaveItem()
        {
            ComplexContainersItem item = new ComplexContainersItem();

            IItemEditor editor = SimulateEditor(item, ItemEditorVersioningMode.SaveOnly);
			DoTheSaving(CreatePrincipal("someone"), editor);

            AssertItemHasValuesFromEditors(item);
        }

		[Test]
		[Obsolete]
        public void NewItem_WithNoChanges_IsSaved()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            item.ID = 0;
            item.MyProperty0 = "one";
            item.MyProperty1 = "two";
            item.MyProperty2 = "three";
            item.MyProperty3 = "rock";
            item.MyProperty4 = true;

            Expect.On(versioner).Call(versioner.SaveVersion(null)).Repeat.Never();
            mocks.Replay(versioner);

            IItemEditor editor = SimulateEditor(item, ItemEditorVersioningMode.VersionAndSave);

			DoTheSaving(CreatePrincipal("someone"), editor);

			Assert.That(item.ID, Is.GreaterThan(0));
        }

		[Test]
		[Obsolete]
        public void SavingWithLimitedPrincipal_DoesntChange_SecuredProperties()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            item.ID = 0;
            item.MyProperty1 = "I'm available";
            item.MyProperty5 = true;
            item.MyProperty6 = "I'm secure!";

			IPrincipal user = CreatePrincipal("Joe");

            Control editorContainer = new Control();
			var added = editManager.AddEditors(definitions.GetDefinition(typeof(ComplexContainersItem)), item, editorContainer, CreatePrincipal("someone")).ToArray();
            Assert.AreEqual(5, added.Length);

            IItemEditor editor = mocks.StrictMock<IItemEditor>();
            using (mocks.Record())
            {
                Expect.Call(editor.CurrentItem).Return(item).Repeat.Any();
                Expect.Call(editor.Editors).Return(added);
                Expect.Call(editor.VersioningMode).Return(ItemEditorVersioningMode.VersionAndSave);
            }

            DoTheSaving(user, editor);

            Assert.AreEqual("", item.MyProperty0);
            Assert.AreEqual("I'm secure!", item.MyProperty6);
            Assert.IsTrue(item.MyProperty5);
        }

        [Test]
        public void WillNotAddOrUpdateEditor_InSecuredContainer_WhenUntrustedUser()
        {
            ItemWithSecuredContainer item = new ItemWithSecuredContainer();
            item.HiddenText = "No way";

			IPrincipal user = CreatePrincipal("Joe", "Editor");

            Control editorContainer = new Control();
            var added = editManager.AddEditors(definitions.GetDefinition(typeof(ItemWithSecuredContainer)), item, editorContainer, user);
			editManager.UpdateEditors(definitions.GetDefinition(item.GetContentType()), item, added, user);

            Assert.That(added.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CanUpdateEditor_InSecuredContainer_WhenUserIsTrusted()
        {
            ItemWithSecuredContainer item = new ItemWithSecuredContainer();
            item.HiddenText = "Yes way";

			IPrincipal user = CreatePrincipal("Joe", "Administrators");

            Control editorContainer = new Control();
            var added = editManager.AddEditors(definitions.GetDefinition(typeof(ItemWithSecuredContainer)), item, editorContainer, user);
			editManager.UpdateEditors(definitions.GetDefinition(item.GetContentType()), item, added, user);

			Assert.That(((TextBox)added.Single(e => e.PropertyName == "HiddenText").Control).Text, Is.EqualTo("Yes way"));
        }

        [Test]
        public void Using_PrivilegedUser_AddsAllEditors()
        {
            ComplexContainersItem item = new ComplexContainersItem();

			IPrincipal user = CreatePrincipal("Joe", "ÜberEditor");

            Control editorContainer = new Control();
            var added = editManager.AddEditors(definitions.GetDefinition(typeof(ComplexContainersItem)), item, editorContainer, user);
            Assert.AreEqual(7, added.Count());
        }

        [Test]
        public void Validators_AreAdded_ToPage()
        {
            Page p = new Page();
            Assert.AreEqual(0, p.Validators.Count);

			editManager.AddEditors(definitions.GetDefinition(typeof(ItemWithRequiredProperty)), new ItemWithRequiredProperty(), p, CreatePrincipal("someone"));

            typeof (Page).GetMethod("InitRecursive", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(p, new[] {p});

            Assert.AreEqual(2, p.Validators.Count);
        }

        [Test]
        public void UpdateEditors_PukesOnNullItem()
        {
			ContainableContext[] editors = CreateEditorsForComplexContainersItem(null);

			Assert.Throws<ArgumentNullException>(() => editManager.UpdateEditors(null, null, editors, null));
        }

        [Test]
        public void UpdateEditors_PukesOnNullAddedEditors()
        {
            ContentItem item = new ComplexContainersItem();

			Assert.Throws<ArgumentNullException>(() => editManager.UpdateEditors(null, item, null, null));
        }

        [Test]
        public void UpdateItem_PukesOnNullItem()
        {
			ContainableContext[] editors = CreateEditorsForComplexContainersItem(null);

			Assert.Throws<ArgumentNullException>(() => editManager.UpdateItem(null, null, editors, null));
        }

        [Test]
        public void UpdateItem_PukesOnNullAddedEditors()
        {
            ContentItem item = new ComplexContainersItem();

			Assert.Throws<ArgumentNullException>(() => editManager.UpdateItem(null, item, null, null));
        }

        [Test]
        public void AppliesModifications()
        {
            Control editorContainer = new Control();
			var added = editManager.AddEditors(definitions.GetDefinition(typeof(ItemWithModification)), new ItemWithModification(), editorContainer, CreatePrincipal("someone"));

            ItemWithModification item = new ItemWithModification();
			editManager.UpdateEditors(definitions.GetDefinition(item.GetContentType()), item, added, CreatePrincipal("someone"));

            TextBox tb = added.Single(e => e.PropertyName == "Essay").Control as TextBox;

            Assert.AreEqual(10, tb.Rows);
            Assert.AreEqual(TextBoxMode.MultiLine, tb.TextMode);
        }

        [Test]
        public void AddingEditor_InvokesEvent()
        {
            Control editorContainer = new Control();
			
			List<Control> noticedByEvent = new List<Control>();
			editManager.AddedEditor += delegate(object sender, N2.Web.UI.ControlEventArgs e) {noticedByEvent.Add(e.Control); };
			var added = editManager.AddEditors(definitions.GetDefinition(typeof(ComplexContainersItem)), new ComplexContainersItem(), editorContainer, CreatePrincipal("someone"));

            Assert.AreEqual(5, noticedByEvent.Count);
            EnumerableAssert.Contains(noticedByEvent, added.Single(c => c.PropertyName == "MyProperty0").Control);
			EnumerableAssert.Contains(noticedByEvent, added.Single(c => c.PropertyName == "MyProperty1").Control);
			EnumerableAssert.Contains(noticedByEvent, added.Single(c => c.PropertyName == "MyProperty2").Control);
			EnumerableAssert.Contains(noticedByEvent, added.Single(c => c.PropertyName == "MyProperty3").Control);
			EnumerableAssert.Contains(noticedByEvent, added.Single(c => c.PropertyName == "MyProperty4").Control);
        }

		[Test]
		[Obsolete]
        public void UpdateItem_WithNoChanges_IsNotSaved()
        {
            ComplexContainersItem item = new ComplexContainersItem();
            item.ID = 22;
            item.MyProperty0 = "one";
            item.MyProperty1 = "two";
            item.MyProperty2 = "three";
            item.MyProperty3 = "rock";
            item.MyProperty4 = true;

			Expect.On(versioner).Call(versioner.SaveVersion(item)).Return(item.Clone(false));
			versioner.Expect(v => v.TrimVersionCountTo(item, 100)).IgnoreArguments().Repeat.Any();
			versioner.Expect(v => v.IsVersionable(item)).Return(true);
            mocks.Replay(versioner);

            IItemEditor editor = SimulateEditor(item, ItemEditorVersioningMode.VersionAndSave);

			DoTheSaving(CreatePrincipal("someone"), editor);

			Assert.That(persister.Repository.Get(22), Is.Null);
        }
    }
}
